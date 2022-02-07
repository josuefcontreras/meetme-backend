import React from "react";
import { Dimmer, Loader, SemanticSIZES } from "semantic-ui-react";

interface Props {
  size?: SemanticSIZES;
  inverted?: boolean;
  content?: string;
}

const LoadingComponent = ({
  size = "massive",
  inverted = true,
  content = "Loading",
}: Props) => {
  return (
    <Dimmer active inverted={inverted}>
      <Loader active size={size}>
        {content}
      </Loader>
    </Dimmer>
  );
};

export default LoadingComponent;
