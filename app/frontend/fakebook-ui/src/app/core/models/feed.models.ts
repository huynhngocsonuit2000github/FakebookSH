export interface CommentData {
  id: string;
  avatar: string;
  name: string;
  username: string;
  time: string;
  text: string;
  likes: number;
}

export interface FeedPost {
  id: string;
  authorId: string;
  author: string;
  username: string;
  avatar: string;
  time: string;
  visibility: string;
  content: string;
  feeling: string | null;
  location: string | null;
  hashtags: string[];
  image: string | null;
  likeCount: number;
  shareCount: number;
  isLiked: boolean;
  isSaved: boolean;
  comments: CommentData[];
}

export interface CreatePostRequest {
  content: string;
  visibility: string;
  feeling?: string | null;
  location?: string | null;
  hashtags?: string[];
  image?: string | null;
}

export interface AddCommentRequest {
  text: string;
}
