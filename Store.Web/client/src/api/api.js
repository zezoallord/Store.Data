// Use relative API base so dev server proxy handles /api -> backend during development
const API_BASE = import.meta.env.VITE_API_BASE ?? ''

export function setToken(token) {
  if (token) localStorage.setItem('token', token)
  else localStorage.removeItem('token')
}
export function getToken() {
  return localStorage.getItem('token')
}

async function safeFetch(url, options = {}) {
  // ensure basic headers and CORS mode are set for browser
  options.mode = options.mode || 'cors'
  options.headers = options.headers || {}
  if (!options.headers['Accept']) options.headers['Accept'] = 'application/json'

  // Attach Bearer token automatically if present and Authorization not already set
  try {
    const token = getToken()
    if (token && !options.headers['Authorization']) {
      options.headers['Authorization'] = `Bearer ${token}`
    }
  } catch (e) {
    // ignore localStorage errors
  }

  try {
    const res = await fetch(url, options)
    return res
  } catch (err) {
    // Network or CORS failure results in a TypeError: Failed to fetch in the browser
    const e = new Error(err.message || 'Network request failed')
    e.isNetworkError = true
    throw e
  }
}

export async function login(email, password) {
  const url = `${API_BASE}/api/account/login`
  const res = await safeFetch(url, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  })
  if (!res.ok) {
    const text = await res.text().catch(() => null)
    const err = new Error(text || `Login failed: ${res.status} ${res.statusText}`)
    err.status = res.status
    throw err
  }
  const data = await res.json()
  if (data?.token) {
    setToken(data.token)
    localStorage.setItem('displayName', data.displayName)
    localStorage.setItem('email', data.email)
  }
  return data
}

export async function register(displayName, email, password) {
  const url = `${API_BASE}/api/account/register`
  const res = await safeFetch(url, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ displayName, email, password })
  })
  if (!res.ok) {
    const text = await res.text().catch(() => null)
    const err = new Error(text || `Registration failed: ${res.status} ${res.statusText}`)
    err.status = res.status
    throw err
  }
  const data = await res.json()
  if (data?.token) {
    setToken(data.token)
    localStorage.setItem('displayName', data.displayName)
    localStorage.setItem('email', data.email)
  }
  return data
}

export async function logout() {
  setToken(null)
  localStorage.removeItem('displayName')
  localStorage.removeItem('email')
  window.location.href = '/'
}

export async function getProducts(params) {
  // params can be an object like { sort: 'priceAsc', search: 'shoe' }
  const url = new URL(`${API_BASE}/api/products/getallproducts`)
  if (params) {
    Object.keys(params).forEach(key => {
      if (params[key]) url.searchParams.append(key, params[key])
    })
  }

  const res = await safeFetch(url.toString())
  if (!res.ok) {
    const err = new Error(`Network response was not ok: ${res.status} ${res.statusText} (url: ${url})`)
    err.status = res.status
    throw err
  }
  const json = await res.json()
  // API returns paginated object { pageIndex, pageSize, totalCount, data }
  if (json && typeof json === 'object' && Array.isArray(json.data)) return json.data
  return json
}

export async function getBasket(id) {
  const res = await safeFetch(`${API_BASE}/api/basket/${id}`)
  if (!res.ok) return null
  return res.json()
}

export async function updateBasket(basket) {
  const res = await safeFetch(`${API_BASE}/api/basket`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(basket)
  })
  if (!res.ok) {
    const text = await res.text().catch(() => null)
    const err = new Error(text || `Failed to update basket: ${res.status} ${res.statusText}`)
    err.status = res.status
    err.responseText = text
    throw err
  }
  return res.json()
}

export async function deleteBasket(id) {
  await safeFetch(`${API_BASE}/api/basket/${id}`, { method: 'DELETE' })
}
