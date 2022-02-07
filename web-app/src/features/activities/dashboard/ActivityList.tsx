import React from "react";
import { Item, Segment } from "semantic-ui-react";
import { Activity } from "../../../app/models/Activity";
import ActivityCard from "./ActivityCard";

interface Props {
  activities: Activity[];
  currentActivityHandler: (id: string) => void;
}

const ActivityList = ({ activities, currentActivityHandler }: Props) => {
  let list = (
    <Segment>
      <Item.Group divided>
        {activities.map((a) => (
          <ActivityCard
            activity={a}
            key={a.id}
            currentActivityHandler={currentActivityHandler}
          />
        ))}
      </Item.Group>
    </Segment>
  );

  return list;
};

export default ActivityList;
