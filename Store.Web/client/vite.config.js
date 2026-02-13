import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path'

// Adjust proxy target if your ASP.NET backend runs on a different origin
const proxyTarget = 'http://localhost:5032'

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: proxyTarget,
        changeOrigin: true,
        secure: false
      }
    }
  },
  build: {
    outDir: path.resolve(__dirname, '../wwwroot/app'),
  }
})
