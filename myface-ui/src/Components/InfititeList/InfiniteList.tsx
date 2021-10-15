import React, {ReactNode, useEffect, useState} from "react";
import {ListResponse} from "../../Api/apiClient";
import {Grid} from "../Grid/Grid";
import "./InfiniteList.scss";

interface InfiniteListProps<T> {
    fetchItems: (page: number, pageSize: number) => Promise<ListResponse<any>>;
    renderItem: (item: any, props: any) => ReactNode;
}

export function InfiniteList<T>(props: InfiniteListProps<T>): JSX.Element {
    const [items, setItems] = useState<any[]>([]);
    const [page, setPage] = useState(1);
    const [hasNextPage, setHasNextPage] = useState(false);

    function replaceItems(response: ListResponse<any>) {
        setItems(response.items);
        setPage(response.page);
        setHasNextPage(response.nextPage !== null);
    }

    function appendItems(response: ListResponse<any>) {
        setItems(items.concat(response.items));
        setPage(response.page);
        setHasNextPage(response.nextPage !== null);
    }

    const updateItemState = (item: any) => {
        const newItems = [...items];
        newItems[newItems.findIndex(i => i.id === item.id)] = item;
        setItems(newItems);
      };
    
    useEffect(() => {
        props.fetchItems(1, 10)
            .then(replaceItems);
    }, [props]);

    function incrementPage() {
        props.fetchItems(page + 1, 10)
            .then(appendItems);
    }
    
    return (
        <div className="infinite-list">
            <Grid>
                {items.map(i => props.renderItem(i, {updatePostState: updateItemState}))}
            </Grid>
            {hasNextPage && <button className="load-more" onClick={incrementPage}>Load More</button>}
        </div>
    );
}