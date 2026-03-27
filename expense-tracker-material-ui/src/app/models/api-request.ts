export interface ApiRequest<T> {
  payload: T;                       // main data you are sending
  metadata?: Record<string, any>;   // optional extra info (e.g., pagination, userId, etc.)
}