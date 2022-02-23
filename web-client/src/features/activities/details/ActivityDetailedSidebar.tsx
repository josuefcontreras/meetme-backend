import React from "react";
import { Segment, List, Label, Item, Image } from "semantic-ui-react";
import { Link } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { Activity } from "../../../app/models/activity";

interface Props {
  activity: Activity;
}

const ActivityDetailedSidebar = ({ activity }: Props) => {
  const { attendees, host } = activity;
  return (
    <>
      <Segment
        textAlign="center"
        style={{ border: "none" }}
        attached="top"
        secondary
        inverted
        color="teal"
      >
        {attendees!.length} {attendees!.length === 1 ? "person" : "people"} going
      </Segment>
      <Segment attached>
        <List relaxed divided>
          {attendees!.map((attendee) => (
            <Item key={attendee.userName} style={{ position: "relative" }}>
              {attendee.userName === host?.userName && (
                <Label style={{ position: "absolute" }} color="orange" ribbon="right">
                  Host
                </Label>
              )}
              <Image size="tiny" src={attendee.image || "/assets/user.png"} />
              <Item.Content verticalAlign="middle">
                <Item.Header as="h3">
                  <Link to={`/profile/${attendee.userName}`}>{attendee.displayName}</Link>
                </Item.Header>
                <Item.Extra style={{ color: "orange" }}>Following</Item.Extra>
              </Item.Content>
            </Item>
          ))}
        </List>
      </Segment>
    </>
  );
};

export default observer(ActivityDetailedSidebar);
