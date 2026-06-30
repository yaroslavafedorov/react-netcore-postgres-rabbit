import type { OrderItem, OrderCreateResponse } from "../types/order";

const BASE_URL = 'http://localhost:5039/api';

export const orderApi = {
    createOrder: async  (items: OrderItem[]) : Promise<string> => {
        const response = await fetch(
            `${BASE_URL}/orders`,
            {
                method: "POST",
                headers: {'Content-Type' : 'application/json'},
                body: JSON.stringify({ items })
            }
        );

        return (await response.json() as OrderCreateResponse).id;
    }
}