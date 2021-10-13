import React, {useEffect, useState} from 'react';
import {useMyFaceApiFunction, User} from "../../Api/apiClient";
import "./UserDetails.scss";

interface UserDetailsProps {
    userId: string;
}

export function UserDetails(props: UserDetailsProps): JSX.Element {
    const [user, setUser] = useState<User | null>(null);
    
    const { fetchUser } = useMyFaceApiFunction();

    useEffect(() => {
        fetchUser(props.userId)
            .then(response => setUser(response));
        return () => {};
    }, [props, fetchUser]);
    
    if (!user) {
        return <section>Loading...</section>
    }
    
    return (
        <section className="user-details">
            <img className="cover-image" src={user.coverImageUrl} alt="User cover."/>
            <div className="user-info">
                <img className="profile-image" src={user.profileImageUrl} alt=""/>
                <div className="contact-info">
                    <h1 className="title">{user.displayName}</h1>
                    <div className="username">{user.username}</div>
                    <div className="email">{user.email}</div>
                </div>
            </div>
        </section>
    );
}