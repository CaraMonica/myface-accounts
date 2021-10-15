import React, {useContext} from "react";
import "./Footer.scss";
import {ILoginContext, LoginContext} from "../LoginManager/LoginManager";

export function Footer(): JSX.Element {
    const loginContext = useContext(LoginContext) as ILoginContext;

    function logout() {
        loginContext.logOut();
    }
    
    return (
        <footer className="footer">
            <p>Made for <a className="link" href="https://techswitch.co.uk">TechSwitch</a>.</p>
            <button className="log-out-button" onClick={logout}>Log Out</button>
        </footer>
    );
}