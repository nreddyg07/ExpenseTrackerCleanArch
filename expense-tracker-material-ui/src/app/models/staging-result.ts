export interface StagingApiResult {
  error?: boolean;
  type: 'upsert' | 'delete';
  response?: boolean; // Changed from ApiResponse<T> to boolean
  message?: string;
}