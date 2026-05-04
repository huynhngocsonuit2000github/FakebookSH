# High-level Home Feed checklist

- Create Post/Feed Service
  - Own posts, feed query, reactions, comments, and saved posts.
- Design database
  - Add tables for posts, media, reactions, comments, and saved posts.
- Build Create Post API
  - Allow authenticated users to create text/media posts.
- Build Home Feed API
  - Return latest posts with author info, media, counts, and viewer state.
- Integrate with BFF
  - Angular calls BFF with cookies; BFF calls Post/Feed Service with JWT.
- Add Angular feed UI integration
  - Load feed, render posts, handle loading/error/empty states, and create posts.
- Add post interactions
  - Support like/unlike, comment, save, and share.
    <!-- - Add pagination -->
      <!-- - Use cursor pagination for infinite scrolling. -->
        <!-- - Add security rules -->
          <!-- - Validate JWT, protect cookie requests with CSRF, and enforce visibility rules. -->
        <!-- - Add logging and monitoring -->
        <!-- - Use correlation ID, structured logs, health checks, and Seq. -->
