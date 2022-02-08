import { observer } from "mobx-react-lite";
import React from "react";
import { Button, Card, Image } from "semantic-ui-react";
import { Activity } from "../../../app/models/Activity";
import { useStore } from "../../../app/stores/storer";

interface Props {
  activity: Activity;
}

const ActivityDetails = ({ activity }: Props) => {
  const { category, title, date, description } = activity;
  const { activityStore } = useStore();
  const { cancelSelectedActivity, openForm } = activityStore;

  return (
    <Card fluid>
      <Image src={`/assets/categoryImages/${category}.jpg`} />
      <Card.Content>
        <Card.Header>{title}</Card.Header>
        <Card.Meta>
          <span className="date">{date}</span>
        </Card.Meta>
        <Card.Description>{description}</Card.Description>
      </Card.Content>
      <Card.Content extra>
        <Button.Group widths={2}>
          <Button
            color="blue"
            content="Edit"
            onClick={() => {
              openForm(activity.id);
            }}
          />
          <Button
            color="grey"
            content="Cancel"
            onClick={() => {
              cancelSelectedActivity();
            }}
          />
        </Button.Group>
      </Card.Content>
    </Card>
  );
};

export default observer(ActivityDetails);
