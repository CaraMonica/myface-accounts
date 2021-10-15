import React, { useContext } from "react";
import {Post, useMyFaceApiFunction} from "../../Api/apiClient";
import {Card} from "../Card/Card";
import "./PostCard.scss";
import {Link} from "react-router-dom";
import {InteractionButton} from "./InteractionButton";
import { ILoginContext, LoginContext } from "../LoginManager/LoginManager";

interface PostCardProps {
    post: Post;
    updatePostState: Function;
}

export function PostCard(props: PostCardProps): JSX.Element {
    const loginContext = useContext(LoginContext) as ILoginContext;
    const { createInteraction, deleteInteraction } = useMyFaceApiFunction();

    const hasUserLiked = props.post.likes.filter(i => i.userId === loginContext.userId).length === 1;
    const onLikeClick = () => createInteraction({userId: loginContext.userId, postId: props.post.id, interactionType: "LIKE"})
                                .then(post => props.updatePostState(post))

    const hasUserDisliked = props.post.dislikes.filter(i => i.userId === loginContext.userId).length === 1;
    const onDislikeClick = () => createInteraction({userId: loginContext.userId, postId: props.post.id, interactionType: "DISLIKE"})
                                .then(post => props.updatePostState(post))

    const onUndoInteraction = () => deleteInteraction({userId: loginContext.userId, postId: props.post.id})
                                    .then(post => props.updatePostState(post))

    return (
        <Card>
            <div className="post-card">
                <img className="image" src={props.post.imageUrl} alt=""/>
                <div className="message">{props.post.message}</div>
                <div className="user">
                    <img className="profile-image" src={props.post.postedBy.profileImageUrl} alt=""/>
                    <Link className="user-name" to={`/users/${props.post.postedBy.id}`}>{props.post.postedBy.displayName}</Link>
                </div>
                <div className="interaction-btn-container">
                    <InteractionButton
                        interactionValue={props.post.likes.length}
                        isLikeButton={true}
                        onClick={hasUserLiked ? onUndoInteraction : onLikeClick}
                        hasUserInteracted={hasUserLiked}
                    />
                    <InteractionButton
                        interactionValue={props.post.dislikes.length}
                        isLikeButton={false}
                        onClick={hasUserDisliked ? onUndoInteraction : onDislikeClick}
                        hasUserInteracted={hasUserDisliked}
                    />
                </div>
            </div>
        </Card>
    );
}