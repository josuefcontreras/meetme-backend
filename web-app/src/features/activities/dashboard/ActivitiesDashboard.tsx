import React from "react";
import { observer } from "mobx-react-lite";
import { Grid } from "semantic-ui-react";
import { useStore } from "../../../app/stores/storer";
import ActivityDetails from "../details/ActivityDetails";
import ActivityForm from "../form/ActivityForm";
import ActivityList from "./ActivityList";

const ActivityDashboard = () => {
  const { activityStore } = useStore();
  const { currentActivity, editMode, activitiesByDate } = activityStore;

  return (
    <Grid>
      <Grid.Column width="10">
        <ActivityList activities={activitiesByDate} />
      </Grid.Column>
      <Grid.Column width="6">
        {currentActivity && !editMode && (
          <ActivityDetails activity={currentActivity} />
        )}
        {editMode && <ActivityForm />}
      </Grid.Column>
    </Grid>
  );
};

export default observer(ActivityDashboard);
