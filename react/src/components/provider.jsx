import React, { createContext, useContext, useState } from 'react';

const MyContext = createContext();
const initialState = {
    // ערכים התחלתיים כאן
  };
const MyProvider = ({ children }) => {
  const [state, setState] = useState(initialState);
  
  return (
    <MyContext.Provider value={{ state, setState }}>
      {children}
    </MyContext.Provider>
  );
};

export default MyProvider;

export const useMyContext = () => useContext(MyContext);
