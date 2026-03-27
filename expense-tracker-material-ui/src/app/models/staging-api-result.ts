// models/staging-api-result.ts
import { ApiResponse } from './api-response';

export interface StagingApiResult<T = any> {
  error?: boolean;
  type: 'upsert' | 'delete';
  response?: ApiResponse<T>;
  message?: string;
}