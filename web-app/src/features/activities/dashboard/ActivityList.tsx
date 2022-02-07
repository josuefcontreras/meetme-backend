import React from "react";
import { Item, Segment } from "semantic-ui-react";
import { Activity } from "../../../app/models/Activity";
import ActivityCard from "./ActivityCard";

interface Props {
  activities: Activity[];
  currentActivityHandler: (id: string) => void;
  handleDeleteActivity: (id: string) => void;
  submitting: boolean;
}

const ActivityList = ({
  activities,
  currentActivityHandler,
  handleDeleteActivity,
  submitting,
}: Props) => {
  let list = (
    <Segment>
      <Item.Group divided>
        {activities.map((a) => (
          <ActivityCard
            activity={a}
            key={a.id}
            currentActivityHandler={currentActivityHandler}
            handleDeleteActivity={handleDeleteActivity}
            submitting={submitting}
          />
        ))}
      </Item.Group>
    </Segment>
  );

  return list;
};

export default ActivityList;
