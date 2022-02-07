import React from "react";
import { Button, Item, Label } from "semantic-ui-react";
import { Activity } from "../../../app/models/Activity";

interface Props {
  activity: Activity;
  currentActivityHandler: (id: string) => void;
}

const ActivityCard = ({ activity, currentActivityHandler }: Props) => {
  let { id, title, date, description, city, venue, category } = activity;
  return (
    <Item>
      <Item.Content>
        <Item.Header as="a">{title}</Item.Header>
        <Item.Meta> {date} </Item.Meta>
        <Item.Description>
          <div>{description}</div>
          <div>
            {city}, {venue}
          </div>
        </Item.Description>
        <Item.Extra>
          <Button
            floated="right"
            content="View"
            color="blue"
            onClick={() => {
              currentActivityHandler(id);
            }}
          />
          <Button
            floated="right"
            content="Delete"
            color="red"
            onClick={() => {
              console.log("Deleting...");
            }}
          />
          <Label basic content={category} />
        </Item.Extra>
      </Item.Content>
    </Item>
  );
};

export default ActivityCard;
