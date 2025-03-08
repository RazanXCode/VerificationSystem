// src/app/redux/store.ts
import { documentReducer } from './reducer';
import { Document } from './type';

// Simple Redux-like Store Implementation
export class Store {
  private state: Document[];
  private listeners: Array<() => void> = [];

  constructor(initialState: Document[]) {
    this.state = initialState;
  }

  // Dispatch function to update the state
  dispatch(action: any) {
    // Use the reducer directly here
    this.state = documentReducer(this.state, action);
    this.listeners.forEach((listener) => listener());
  }

  // Get the current state
  getState() {
    return this.state;
  }

  // Subscribe to state changes, listener is a function to notify
  subscribe(listener: () => void) {
    this.listeners.push(listener);
  }
}

// Initialize the store with an empty array of documents
export const store = new Store([]);
