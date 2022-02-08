import { observer } from "mobx-react-lite";
import React, { ChangeEvent, useState } from "react";
import { Button, Form, Segment } from "semantic-ui-react";
import { Activity } from "../../../app/models/Activity";
import { useStore } from "../../../app/stores/storer";

const ActivityForm = () => {
  const { activityStore } = useStore();
  const {
    currentActivity,
    isSubmitting,
    createActivity,
    editActivity,
    closeForm,
  } = activityStore;
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

  function handelSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    activity.id === "" ? createActivity(activity) : editActivity(activity);
  }

  function handleInputChange({
    target,
  }: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
    var newActivity = { ...activity, [`${target.name}`]: target.value };
    setActivity(newActivity);
  }

  return (
    <Segment clearing>
      <Form
        onSubmit={(e) => {
          handelSubmit(e);
        }}
      >
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
          type="date"
          placeholder="Date"
          name="date"
          value={activity.date.split("T")[0]}
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
        <Button
          floated="right"
          positive
          type="submit"
          content="Submit"
          loading={isSubmitting}
        />
        <Button
          floated="right"
          content="Cancel"
          onClick={() => {
            closeForm();
          }}
        />
      </Form>
    </Segment>
  );
};

export default observer(ActivityForm);
