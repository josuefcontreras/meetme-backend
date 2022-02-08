import { observer } from "mobx-react-lite";
import React, { useState } from "react";
import { Button, Item, Label } from "semantic-ui-react";
import { Activity } from "../../../app/models/Activity";
import { useStore } from "../../../app/stores/storer";

interface Props {
  activity: Activity;
}

const ActivityCard = ({ activity }: Props) => {
  const { id, title, date, description, city, venue, category } = activity;
  const [target, setTarget] = useState("");
  const { activityStore } = useStore();
  const { selectActivity, deleteActivity, isSubmitting } = activityStore;

  function handleDeleteButtonClick(
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>,
    id: string
  ) {
    setTarget(e.currentTarget.name);
    deleteActivity(id);
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
              selectActivity(id);
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
            loading={target === id ? isSubmitting : false}
          />
          <Label basic content={category} />
        </Item.Extra>
      </Item.Content>
    </Item>
  );
};

export default observer(ActivityCard);
