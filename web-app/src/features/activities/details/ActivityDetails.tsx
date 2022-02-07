import React from "react";
import { Button, ButtonGroup, Card, Image } from "semantic-ui-react";
import { Activity } from "../../../app/models/Activity";

interface Props {
  activity: Activity;
  cancelCurrentActivityHandler: () => void;
  handleFormOpen: (id: string) => void;
}

const ActivityDetails = ({
  activity,
  cancelCurrentActivityHandler,
  handleFormOpen,
}: Props) => {
  let { category, title, date, description } = activity;

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
              handleFormOpen(activity.id);
            }}
          />
          <Button
            color="grey"
            content="Cancel"
            onClick={() => {
              cancelCurrentActivityHandler();
            }}
          />
        </Button.Group>
      </Card.Content>
    </Card>
  );
};

export default ActivityDetails;
