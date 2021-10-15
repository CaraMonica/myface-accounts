import React, { MouseEventHandler } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faThumbsDown, faThumbsUp } from "@fortawesome/free-solid-svg-icons";

interface InteractionButtonProps {
  isLikeButton: boolean;
  onClick: MouseEventHandler<SVGSVGElement>;
  hasUserInteracted: boolean,
  interactionValue: number,
}

interface LikeDislikeButtonProps {
  onClick: MouseEventHandler<SVGSVGElement>,
  interactionType: string,
  hasUserInteracted: boolean,
  interactionValue: number,
  buttonType: any,
}

const LikeDislikeButton = (props: LikeDislikeButtonProps) => (
  <div>
    <FontAwesomeIcon
      icon={props.buttonType}
      onClick={props.onClick}
      className={`btn ${props.interactionType}${props.hasUserInteracted ? ` ${props.interactionType}d` : ""}`}
    />
    <div className="interaction-value">{props.interactionValue}</div>
  </div>
);

export const InteractionButton = (props: InteractionButtonProps) => (
  props.isLikeButton ? (
    <LikeDislikeButton
      onClick={props.onClick}
      interactionType="like"
      hasUserInteracted={props.hasUserInteracted}
      interactionValue={props.interactionValue}
      buttonType={faThumbsUp}
    />
  ) : (
    <LikeDislikeButton
      onClick={props.onClick}
      interactionType="dislike"
      hasUserInteracted={props.hasUserInteracted}
      interactionValue={props.interactionValue}
      buttonType={faThumbsDown}
    />
  )
);
