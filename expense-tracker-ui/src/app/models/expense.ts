export interface Expense {
  id?: number;
  title: string;
  amt: number;
  category?: string | null;
  date: string; // ISO date string
}
