import React, {FormEvent, useContext, useState} from 'react';
import {Page} from "../Page/Page";
import {ILoginContext, LoginContext} from "../../Components/LoginManager/LoginManager";
import "./Login.scss";
import { login } from '../../Api/apiClient';

export function Login(): JSX.Element {
    const loginContext = useContext(LoginContext) as ILoginContext;
    
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [unsuccessfulLogin, setUnsuccessfulLogin] = useState(false);
    
    async function tryLogin(event: FormEvent) {
        event.preventDefault();

        let id;
        let loggedIn = true;
 
        try {

            ({ id } = await login(username, password))
        } 
        catch (e) {
            loggedIn = false;
            console.error(e);
        }

        if (!loggedIn) {
            setUnsuccessfulLogin(true);
            return;
        }

        loginContext.setUsername(username);
        loginContext.setPassword(password);
        loginContext.setUserId(id);
        loginContext.logIn();
    }
    
    return (
        <Page containerClassName="login">
            <h1 className="title">Log In</h1>
            {unsuccessfulLogin && <p className="error-message">Invalid username or password..</p>}
            <form className="login-form" onSubmit={tryLogin}>
                <label className="form-label">
                    Username
                    <input className="form-input" type={"text"} value={username} onChange={event => setUsername(event.target.value)}/>
                </label>

                <label className="form-label">
                    Password
                    <input className="form-input" type={"password"} value={password} onChange={event => setPassword(event.target.value)}/>
                </label>
                
                <button className="submit-button" type="submit">Log In</button>
            </form>
        </Page>
    );
}