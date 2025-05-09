const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
    ],
    target: "https://localhost:7053",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
