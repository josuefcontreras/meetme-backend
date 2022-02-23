import React from "react";
import { Card, Icon, Image } from "semantic-ui-react";
import { Profile } from "../../app/models/profile";

interface Props {
  profile: Profile;
}

const ProfileCard = ({ profile }: Props) => {
  return (
    <Card>
      <Image src={profile.image || "/assets/user.png"} size="small" />
      <Card.Content>
        <Card.Header>{profile.displayName}</Card.Header>
        <Card.Description>{"Bio goes here..."}</Card.Description>
      </Card.Content>
      <Card.Content extra>
        <Icon name="user" />
        20 followers
      </Card.Content>
    </Card>
  );
};

export default ProfileCard;
