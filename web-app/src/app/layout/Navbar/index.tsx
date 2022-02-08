import React from "react";
import { Button, Menu } from "semantic-ui-react";
import { useStore } from "../../stores/storer";

const Navbar = () => {
  const { activityStore } = useStore();
  const { openForm } = activityStore;

  return (
    <Menu inverted fixed="top">
      <Menu.Item header>
        <img src="/assets/logo.png" alt="logo" style={{ marginRight: 10 }} />
        MeetMe
      </Menu.Item>
      <Menu.Item name="All meetings" />
      <Menu.Item header>
        <Button
          positive
          content="Create meeting"
          onClick={() => {
            openForm();
          }}
        />
      </Menu.Item>
    </Menu>
  );
};

export default Navbar;
