// src/app/redux/reducer.ts
import { Document } from './type';
import {
  FETCH_DOCUMENTS,
  UPLOAD_DOCUMENT,
  VERIFY_DOCUMENT
} from './actions';

// Reducer with Document type
export const documentReducer = (state: Document[] = [], action: any): Document[] => {
  switch (action.type) {
    case FETCH_DOCUMENTS:
      return action.payload;
    case UPLOAD_DOCUMENT:
      return [...state, action.payload];
    case VERIFY_DOCUMENT:
      return state.map((doc) =>
        doc.id === action.payload ? { ...doc, verified: true } : doc
      );
    default:
      return state;
  }
};
