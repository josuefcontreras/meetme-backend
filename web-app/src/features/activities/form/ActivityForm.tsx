import React, { ChangeEvent, useState } from "react";
import { Button, Form, Segment } from "semantic-ui-react";
import { Activity } from "../../../app/models/Activity";

interface Props {
  currentActivity: Activity | undefined;
  handleFormClose: () => void;
}

const ActivityForm = ({ currentActivity, handleFormClose }: Props) => {
  const initialFormState =
    currentActivity ??
    ({
      id: "",
      title: "",
      description: "",
      category: "",
      date: "",
      city: "",
      venue: "",
    } as Activity);

  const [activity, setActivity] = useState(initialFormState);

  function handelSubmit() {
    console.log(activity);
  }

  function handleInputChange({
    target,
  }: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
    var newActivity = { ...activity, [`${target.name}`]: target.value };
    setActivity(newActivity);
  }

  return (
    <Segment clearing>
      <Form onSubmit={handelSubmit}>
        <Form.Input
          placeholder="Title"
          name="title"
          value={activity.title}
          onChange={(e) => {
            handleInputChange(e);
          }}
        />
        <Form.TextArea
          placeholder="Description"
          name="description"
          value={activity.description}
          onChange={(e) => {
            handleInputChange(e);
          }}
        />
        <Form.Input
          placeholder="Category"
          name="category"
          value={activity.category}
          onChange={(e) => {
            handleInputChange(e);
          }}
        />
        <Form.Input
          placeholder="Date"
          name="date"
          value={activity.date}
          onChange={(e) => {
            handleInputChange(e);
          }}
        />
        <Form.Input
          placeholder="City"
          name="city"
          value={activity.city}
          onChange={(e) => {
            handleInputChange(e);
          }}
        />
        <Form.Input
          placeholder="Venue"
          name="venue"
          value={activity.venue}
          onChange={(e) => {
            handleInputChange(e);
          }}
        />
        <Button floated="right" positive type="submit" content="Submit" />
        <Button
          floated="right"
          type="submit"
          content="Cancel"
          onClick={() => {
            handleFormClose();
          }}
        />
      </Form>
    </Segment>
  );
};

export default ActivityForm;
