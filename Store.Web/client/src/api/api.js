// Use relative API base so dev server proxy handles /api -> backend during development
const API_BASE = import.meta.env.VITE_API_BASE ?? ''

export function setToken(token){
  if (token) localStorage.setItem('token', token)
  else localStorage.removeItem('token')
}
export function getToken(){
  return localStorage.getItem('token')
}

async function safeFetch(url, options = {}){
  // ensure basic headers and CORS mode are set for browser
  options.mode = options.mode || 'cors'
  options.headers = options.headers || {}
  if (!options.headers['Accept']) options.headers['Accept'] = 'application/json'
  try{
    const res = await fetch(url, options)
    return res
  }catch(err){
    // Network or CORS failure results in a TypeError: Failed to fetch in the browser
    const e = new Error(err.message || 'Network request failed')
    e.isNetworkError = true
    throw e
  }
}

export async function login(email, password){
  const url = `${API_BASE}/api/account/login`
  const res = await safeFetch(url, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  })
  if(!res.ok) {
    const text = await res.text().catch(()=>null)
    const err = new Error(text || `Login failed: ${res.status} ${res.statusText}`)
    err.status = res.status
    throw err
  }
  const data = await res.json()
  if (data?.token) setToken(data.token)
  return data
}

export async function logout(){
  setToken(null)
}

export async function getProducts(){
  const url = `${API_BASE}/api/products/getallproducts`
  const headers = {}
  const token = getToken()
  if (token) headers['Authorization'] = `Bearer ${token}`
  const res = await safeFetch(url, { headers })
  if(!res.ok) {
    const err = new Error(`Network response was not ok: ${res.status} ${res.statusText} (url: ${url})`)
    err.status = res.status
    throw err
  }
  const json = await res.json()
  // API returns paginated object { pageIndex, pageSize, totalCount, data }
  if (json && typeof json === 'object' && Array.isArray(json.data)) return json.data
  return json
}
