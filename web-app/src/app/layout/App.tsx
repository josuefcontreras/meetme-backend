import React, { useEffect, useState } from "react";
import axios from "axios";
import { v4 as uuid } from "uuid";
import { Container } from "semantic-ui-react";
import ActivityDashboard from "../../features/activities/dashboard/ActivitiesDashboard";
import { Activity } from "../models/Activity";
import Navbar from "./Navbar";
import "./styles.css";

function App() {
  const [activities, setActivities] = useState<Activity[]>([]);
  const [editMode, setEditMode] = useState(false);
  const [currentActivity, setCurrentActivity] = useState<Activity | undefined>(
    undefined
  );

  useEffect(() => {
    async function activityFetch() {
      var response = await axios.get<Activity[]>(
        "https://localhost:5001/api/activities"
      );
      setActivities(response.data);
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
        />
      </Container>
    </div>
  );
}

export default App;
