# High-level Home Feed checklist

- [x] Add image in the post
- [x] Create Post/Feed Service
  - [x] Own posts, feed query, reactions, comments, and saved posts.
- [x] Design database
  - [x] Add tables for posts, media, reactions, comments, and saved posts.
- [x] Build Create Post API
  - [x] Allow authenticated users to create text/media posts.
- [x] Build Home Feed API
  - [x] Return latest posts with author info, media, counts, and viewer state.
- [x] Integrate with BFF
  - [x] Angular calls BFF with cookies; BFF calls Post/Feed Service with JWT.
- [x] Add Angular feed UI integration
  - [x] Load feed, render posts, handle loading/error/empty states, and create posts.
- [x] Add post interactions
  - [x] Support like/unlike, comment, save, and share.
- [x] Add pagination
  - [x] Use cursor pagination for infinite scrolling.
