# Integrate with UI

- [x] Change core to use cookies base for BFF, but JWT for calling downstream system, use Private and public key
- [x] Cookie stores only session id, Tokens are stored server-side, for example Redis
  - [x] before apply: 2571
  - [x] after apply: 323
- [x] Apply redis database for storing cookies ticket information, and just return the session id to the browser
- [x] Create redis core in the building blocks fore future using ICacheService
