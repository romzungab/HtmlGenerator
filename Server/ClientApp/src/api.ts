async function get<T>(url: string) {
    const resp = await fetch('/api/' + url);
    return await resp.json() as T;
}

export async function getFactTables() {
    return await get<string[]>('facttables');
}
