import React, { useState } from "react";
import { Grid } from "semantic-ui-react";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { Activity } from "../../../app/models/Activity";
import ActivityDetails from "../details/ActivityDetails";
import ActivityForm from "../form/ActivityForm";
import ActivityList from "./ActivityList";

interface Props {
  activities: Activity[];
  currentActivity: Activity | undefined;
  editMode: boolean;
  currentActivityHandler: (id: string) => void;
  cancelCurrentActivityHandler: () => void;
  handleFormOpen: (id: string) => void;
  handleFormClose: () => void;
  handleDeleteActivity: (id: string) => void;
  handleCreatOrEditActivity: (activity: Activity) => void;
  submitting: boolean;
}

const ActivityDashboard = ({
  activities,
  currentActivity,
  editMode,
  currentActivityHandler,
  cancelCurrentActivityHandler,
  handleFormOpen,
  handleFormClose,
  handleDeleteActivity,
  handleCreatOrEditActivity,
  submitting,
}: Props) => {
  return (
    <Grid>
      <Grid.Column width="10">
        <ActivityList
          activities={activities}
          currentActivityHandler={currentActivityHandler}
          handleDeleteActivity={handleDeleteActivity}
          submitting={submitting}
        />
      </Grid.Column>
      <Grid.Column width="6">
        {currentActivity && !editMode ? (
          <ActivityDetails
            activity={currentActivity}
            cancelCurrentActivityHandler={cancelCurrentActivityHandler}
            handleFormOpen={handleFormOpen}
          />
        ) : null}
        {editMode ? (
          <ActivityForm
            currentActivity={currentActivity}
            handleFormClose={handleFormClose}
            handleCreatOrEditActivity={handleCreatOrEditActivity}
            submitting={submitting}
          />
        ) : null}
      </Grid.Column>
    </Grid>
  );
};

export default ActivityDashboard;
