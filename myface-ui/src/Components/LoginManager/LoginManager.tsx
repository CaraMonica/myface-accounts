import React, {createContext, ReactNode, useState} from "react";

export interface ILoginContext {
    isLoggedIn: boolean,
    isAdmin: boolean,
    username: string,
    setUsername: Function,
    password: string,
    setPassword: Function,
    userId: number,
    setUserId: Function,
    logIn: Function,
    logOut: Function,
};

export const LoginContext = createContext<ILoginContext | null>(null);

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(false);
    const [username, setUsername] = useState("");
    const [userId, setUserId] = useState(-1);
    const [password, setPassword] = useState("");

    
    function logIn() {
        setLoggedIn(true);
    }
    
    function logOut() {
        setLoggedIn(false);
    }
    
    const context = {
        isLoggedIn: loggedIn,
        isAdmin: loggedIn,
        username: username,
        setUsername: setUsername,
        password: password,
        setPassword: setPassword,
        userId: userId,
        setUserId: setUserId,
        logIn: logIn,
        logOut: logOut,
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}