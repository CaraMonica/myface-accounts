import React, {createContext, ReactNode, useState} from "react";

export const LoginContext = createContext({
    isLoggedIn: false,
    isAdmin: false,
    username: "",
    setUsername: (username: string) => {},
    password: "",
    setPassword: (password: string) => {},
    logIn: () => {},
    logOut: () => {},
});

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(false);
    const [username, setUsername] = useState("");
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
        logIn: logIn,
        logOut: logOut,
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}