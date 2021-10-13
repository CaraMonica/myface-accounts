import React, {useState} from "react";
import {Page} from "../Page/Page";
import {SearchInput} from "../../Components/SearchInput/SearchInput";
import {useMyFaceApiFunction} from "../../Api/apiClient";
import {UserCard} from "../../Components/UserCard/UserCard";
import {InfiniteList} from "../../Components/InfititeList/InfiniteList";
import "./Users.scss";

export function Users(): JSX.Element {
    const [searchTerm, setSearchTerm] = useState("");

    const { fetchUsers } = useMyFaceApiFunction();

    function getUsers(page: number, pageSize: number) {
        return fetchUsers(searchTerm, page, pageSize);
    }
    
    return (
        <Page containerClassName="users">
            <h1 className="title">Users</h1>
            <SearchInput searchTerm={searchTerm} updateSearchTerm={setSearchTerm}/>
            <InfiniteList fetchItems={getUsers} renderItem={user => <UserCard key={user.id} user={user}/>}/>
        </Page>
    );
}