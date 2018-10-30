export interface Dimension{
    name : string,
    attributes : string[]
}

export interface FactTable{
    name: string,
    dimensions: Dimension[],
}
async function get<T>(url: string) {
    const resp = await fetch('/api/' + url);
    return await resp.json() as T;
}

export async function getFactTables() {
    return await get<FactTable[]>('facttables');
}

export async function getDimensionAttributes() {
    return await get<string[]>('dimensionattributes');
}
