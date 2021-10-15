import { useContext } from "react";
import { ILoginContext, LoginContext } from "../Components/LoginManager/LoginManager";

export interface ListResponse<T> {
    items: T[];
    totalNumberOfItems: number;
    page: number;
    nextPage: string;
    previousPage: string;
}

export interface User {
    id: number;
    firstName: string;
    lastName: string;
    displayName: string;
    username: string;
    email: string;
    profileImageUrl: string;
    coverImageUrl: string;
}

export interface Interaction {
    postId: number;
    userId: number;
    interactionType: string;
}

export interface Post {
    id: number;
    message: string;
    imageUrl: string;
    postedAt: string;
    postedBy: User;
    likes: Interaction[];
    dislikes: Interaction[];
}

export interface NewPost {
    message: string;
    imageUrl: string;
}

const useBasicAuthFetch = () => {
    const loginContext = useContext(LoginContext) as ILoginContext;

    const fetchWithBasicAuth: any = async (url: string, init?: any) => {
        const initWithAuthHeader = {
            ...init,
            headers: {
                ...init?.headers,
                Authorization: `Basic ${btoa(`${loginContext.username}:${loginContext.password}`)}`,
            },
        };

        const response = await fetch(url, initWithAuthHeader);

        if (response.status === 401) loginContext.logOut();

        return response;
    };

    return fetchWithBasicAuth;
};

export const useMyFaceApiFunction = () => {
    const fetchWithBasicAuth = useBasicAuthFetch();

    const fetchUsers = async (
        searchTerm: string,
        page: number,
        pageSize: number
    ): Promise<ListResponse<User>> => {
        const response = await fetchWithBasicAuth(
            `https://localhost:5001/users?search=${searchTerm}&page=${page}&pageSize=${pageSize}`
        );
        return await response.json();
    };

    const fetchUser = async (userId: string | number): Promise<User> => {
        const response = await fetchWithBasicAuth(`https://localhost:5001/users/${userId}`);
        return await response.json();
    };

    const fetchPosts = async (page: number, pageSize: number): Promise<ListResponse<Post>> => {
        const response = await fetchWithBasicAuth(
            `https://localhost:5001/feed?page=${page}&pageSize=${pageSize}`
        );
        return await response.json();
    };

    const fetchPostsForUser = async (page: number, pageSize: number, userId: string | number) => {
        const response = await fetchWithBasicAuth(
            `https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&postedBy=${userId}`
        );
        return await response.json();
    };

    const fetchPostsLikedBy = async (page: number, pageSize: number, userId: string | number) => {
        const response = await fetchWithBasicAuth(
            `https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&likedBy=${userId}`
        );
        return await response.json();
    };

    const fetchPostsDislikedBy = async (page: number, pageSize: number, userId: string | number) => {
        const response = await fetchWithBasicAuth(
            `https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&dislikedBy=${userId}`
        );
        return await response.json();
    };

    const createPost = async (newPost: NewPost) => {
        const response = await fetchWithBasicAuth(`https://localhost:5001/posts/create`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(newPost),
        });

        if (!response.ok) {
            throw new Error(await response.json());
        }
    };

    const createInteraction = async (newInteraction: Interaction) => {
        const response = await fetchWithBasicAuth(`https://localhost:5001/interactions/create`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(newInteraction),
        });

        if (!response.ok) {
            throw new Error(await response.json());
        }

        return await response.json();
    };

    return {
        createInteraction,
        createPost,
        fetchPostsDislikedBy,
        fetchPostsLikedBy,
        fetchPostsForUser,
        fetchPosts,
        fetchUser,
        fetchUsers,
    };
};

export const login = async (username: string, password: string) => {
    const response = await fetch(`https://localhost:5001/login`, {
        headers: {
            Authorization: `Basic ${btoa(`${username}:${password}`)}`,
        },
    });

    if (!response.ok) 
    {
        throw new Error(await response.json());
    }

    const json = await response.json();
    console.log(json)
    return json;
};
