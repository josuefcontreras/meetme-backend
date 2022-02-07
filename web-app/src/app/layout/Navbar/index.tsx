import React from "react";
import { Button, Menu } from "semantic-ui-react";

interface Props {
  handleFormOpen: () => void;
}
const Navbar = ({ handleFormOpen }: Props) => {
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
            handleFormOpen();
          }}
        />
      </Menu.Item>
    </Menu>
  );
};

export default Navbar;
