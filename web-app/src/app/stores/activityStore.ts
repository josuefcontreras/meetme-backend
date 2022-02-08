import { makeAutoObservable } from "mobx";
import API_AGENT from "../api/api_agent";
import { Activity } from "../models/Activity";
import { v4 as uuid } from "uuid";

class ActivityStore {
  activityRegistry = new Map<string, Activity>();
  currentActivity: Activity | undefined = undefined;
  isLoadingActivities: boolean = false;
  editMode: boolean = false;
  isSubmitting: boolean = false;

  constructor() {
    makeAutoObservable(this);
  }

  get activitiesByDate() {
    const activitiesArray = Array.from(this.activityRegistry.values());

    const sortedActivitiesArray = activitiesArray.sort(sortByDate);

    function sortByDate(a: Activity, b: Activity) {
      return Date.parse(a.date) - Date.parse(b.date);
    }

    return sortedActivitiesArray;
  }

  loadActivities = async () => {
    this.setIsLoadingActivities(true);
    try {
      var response = await API_AGENT.Activities.list();
      this.setActivities(response);
      this.setIsLoadingActivities(false);
    } catch (error) {
      this.setIsLoadingActivities(false);
      console.log(error);
    }
  };

  setIsLoadingActivities = (isLoading: boolean) => {
    this.isLoadingActivities = isLoading;
  };

  setIsSubmitting = (isSubmitting: boolean) => {
    this.isSubmitting = isSubmitting;
  };

  setActivities = (activities: Activity[]) => {
    activities.forEach((a) => this.activityRegistry.set(a.id, a));
  };

  setCurrentActivity = (activity: Activity | undefined) => {
    this.currentActivity = activity;
  };

  setEditMode = (isEditing: boolean) => {
    this.editMode = isEditing;
  };

  selectActivity = (id: string) => {
    this.setCurrentActivity(this.activityRegistry.get(id));
  };

  cancelSelectedActivity = () => {
    this.setCurrentActivity(undefined);
  };

  openForm = (id?: string) => {
    id ? this.selectActivity(id) : this.cancelSelectedActivity();
    this.setEditMode(true);
  };

  closeForm = () => {
    this.setEditMode(false);
  };

  createActivity = async (activity: Activity) => {
    this.setIsSubmitting(true);
    activity.id = uuid();
    try {
      await API_AGENT.Activities.create(activity);
      this.activityRegistry.set(activity.id, activity);
      this.setCurrentActivity(activity);
      this.setEditMode(false);
      this.setIsSubmitting(false);
    } catch (error) {
      console.log(error);
      this.setIsSubmitting(false);
    }
  };

  editActivity = async (activity: Activity) => {
    this.setIsSubmitting(true);
    try {
      await API_AGENT.Activities.edit(activity);
      this.activityRegistry.set(activity.id, activity);
      this.setCurrentActivity(activity);
      this.setEditMode(false);
      this.setIsSubmitting(false);
    } catch (error) {
      console.log(error);
      this.setIsSubmitting(false);
    }
  };

  deleteActivity = async (id: string) => {
    this.setIsSubmitting(true);
    try {
      await API_AGENT.Activities.delete(id);
      this.activityRegistry.delete(id);
      this.setIsSubmitting(false);

      if (id === this.currentActivity?.id) {
        this.cancelSelectedActivity();
      }
    } catch (error) {
      this.setIsSubmitting(false);
      console.log(error);
    }
  };
}

export default ActivityStore;
