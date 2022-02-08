import { observer } from "mobx-react-lite";
import React from "react";
import { Item, Segment } from "semantic-ui-react";
import { Activity } from "../../../app/models/Activity";
import ActivityCard from "./ActivityCard";

interface Props {
  activities: Activity[];
}

const ActivityList = ({ activities }: Props) => {
  let list = (
    <Segment>
      <Item.Group divided>
        {activities.map((a) => (
          <ActivityCard activity={a} key={a.id} />
        ))}
      </Item.Group>
    </Segment>
  );

  return list;
};

export default observer(ActivityList);
