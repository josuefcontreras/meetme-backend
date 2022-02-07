import React, { useEffect, useState } from "react";
import { Container } from "semantic-ui-react";
import ActivityDashboard from "../../features/activities/dashboard/ActivitiesDashboard";
import { Activity } from "../models/Activity";
import Navbar from "./Navbar";
import "./styles.css";
import API_AGENT from "../api/api_agent";
import LoadingComponent from "./LoadingComponent";
import { v4 as uuid } from "uuid";

function App() {
  const [activities, setActivities] = useState<Activity[]>([]);
  const [isLoadingApp, setisLoadingApp] = useState(true);
  const [editMode, setEditMode] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [currentActivity, setCurrentActivity] = useState<Activity | undefined>(
    undefined
  );

  useEffect(() => {
    async function activityFetch() {
      var response = await API_AGENT.Activities.list();
      setActivities(response);
      setisLoadingApp(false);
    }
    activityFetch();
  }, []);

  function currentActivityHandler(id: string) {
    setCurrentActivity(activities.find((a) => a.id === id));
  }

  function cancelCurrentActivityHandler() {
    setCurrentActivity(undefined);
  }

  function handleFormOpen(id?: string) {
    id ? currentActivityHandler(id) : cancelCurrentActivityHandler();
    setEditMode(true);
  }

  function handleFormClose() {
    setEditMode(false);
  }

  async function handleCreatOrEditActivity(activity: Activity) {
    setSubmitting(true);

    if (activity.id !== "") {
      await API_AGENT.Activities.edit(activity);
      setActivities([
        ...activities.filter((a) => a.id !== activity.id),
        activity,
      ]);
      setCurrentActivity(activity);
      setEditMode(false);
      setSubmitting(false);
    } else {
      activity.id = uuid();
      await API_AGENT.Activities.create(activity);
      setActivities([...activities, activity]);
      setCurrentActivity(activity);
      setEditMode(false);
      setSubmitting(false);
    }
  }

  async function handleDeleteActivity(id: string) {
    setSubmitting(true);
    await API_AGENT.Activities.delete(id);
    setActivities([...activities.filter((a) => a.id !== id)]);
    setSubmitting(false);
  }

  if (isLoadingApp) return <LoadingComponent />;

  return (
    <div className="App-container">
      <Navbar handleFormOpen={handleFormOpen} />
      <Container style={{ marginTop: "7em" }}>
        <ActivityDashboard
          activities={activities}
          currentActivity={currentActivity}
          editMode={editMode}
          currentActivityHandler={currentActivityHandler}
          cancelCurrentActivityHandler={cancelCurrentActivityHandler}
          handleFormOpen={handleFormOpen}
          handleFormClose={handleFormClose}
          handleDeleteActivity={handleDeleteActivity}
          handleCreatOrEditActivity={handleCreatOrEditActivity}
          submitting={submitting}
        />
      </Container>
    </div>
  );
}

export default App;
