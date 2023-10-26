import { defineConfig } from "vite";

import react from "@vitejs/plugin-react";

// https://vitejs.dev/config/

export default defineConfig({
  server: {
    host: "0.0.0.0",

    watch: {
      usePolling: true,
    },

    reactRefresh: false,
  },

  test: {
    globals: true,

    environment: "jsdom",
  },

  plugins: [react()],
});
