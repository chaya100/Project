import React from 'react';
import { Box, CircularProgress } from '@mui/material';
import { circularProgressClasses } from '@mui/material/CircularProgress';

export default function FacebookCircularProgress(props) {
  return (
    <Box sx={{ position: 'relative', display: 'inline-flex' }}>
      <CircularProgress
        variant="determinate"
        sx={(theme) => ({
          color: theme.palette.grey[200],
        })}
        size={40}
        thickness={4}
        value={100}
        {...props}
      />
      <CircularProgress
        variant="indeterminate"
        disableShrink
        sx={{
          color: '#1a90ff',
          animationDuration: '550ms',
          position: 'absolute',
          left: 0,
          [`& .${circularProgressClasses.circle}`]: {
            strokeLinecap: 'round',
          },
        }}
        size={40}
        thickness={4}
        {...props}
      />
    </Box>
  );
}
