import React, { useEffect } from "react";
import { Container } from "semantic-ui-react";
import ActivityDashboard from "../../features/activities/dashboard/ActivitiesDashboard";
import Navbar from "./Navbar";
import "./styles.css";
import LoadingComponent from "./LoadingComponent";
import { useStore } from "../stores/storer";
import { observer } from "mobx-react-lite";

function App() {
  const { activityStore } = useStore();
  const { isLoadingActivities } = activityStore;

  useEffect(() => {
    activityStore.loadActivities();
  }, [activityStore]);

  if (isLoadingActivities) return <LoadingComponent />;

  return (
    <div className="App-container">
      <Navbar />
      <Container style={{ marginTop: "7em" }}>
        <ActivityDashboard />
      </Container>
    </div>
  );
}

export default observer(App);
