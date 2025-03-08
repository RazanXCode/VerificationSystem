// src/app/redux/actions.ts
import { Document } from './type';

// Redux Actions
export const FETCH_DOCUMENTS = 'FETCH_DOCUMENTS';
export const UPLOAD_DOCUMENT = 'UPLOAD_DOCUMENT';
export const VERIFY_DOCUMENT = 'VERIFY_DOCUMENT';

// Action Creators
export const fetchDocuments = (documents: Document[]) => ({
  type: FETCH_DOCUMENTS,
  payload: documents
});

export const uploadDocument = (document: Document) => ({
  type: UPLOAD_DOCUMENT,
  payload: document
});

export const verifyDocument = (id: number) => ({
  type: VERIFY_DOCUMENT,
  payload: id
  
});

// src/app/redux/actions.ts

export const UPLOAD_DOCUMENT_SUCCESS = 'UPLOAD_DOCUMENT_SUCCESS';
export const UPLOAD_DOCUMENT_FAILURE = 'UPLOAD_DOCUMENT_FAILURE';

export const uploadDocumentSuccess = (document: any) => ({
  type: UPLOAD_DOCUMENT_SUCCESS,
  payload: document,
});

export const uploadDocumentFailure = (error: any) => ({
  type: UPLOAD_DOCUMENT_FAILURE,
  payload: error,
});
