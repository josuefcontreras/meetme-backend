import React, { useState } from "react";
import { Button, Item, Label } from "semantic-ui-react";
import { Activity } from "../../../app/models/Activity";

interface Props {
  activity: Activity;
  currentActivityHandler: (id: string) => void;
  handleDeleteActivity: (id: string) => void;
  submitting: boolean;
}

const ActivityCard = ({
  activity,
  currentActivityHandler,
  handleDeleteActivity,
  submitting,
}: Props) => {
  const { id, title, date, description, city, venue, category } = activity;
  const [target, setTarget] = useState("");

  function handleDeleteButtonClick(
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>,
    id: string
  ) {
    setTarget(e.currentTarget.name);
    handleDeleteActivity(id);
  }

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
            name={id}
            floated="right"
            content="Delete"
            color="red"
            onClick={(e) => {
              handleDeleteButtonClick(e, id);
            }}
            loading={target === id ? submitting : false}
          />
          <Label basic content={category} />
        </Item.Extra>
      </Item.Content>
    </Item>
  );
};

export default ActivityCard;
