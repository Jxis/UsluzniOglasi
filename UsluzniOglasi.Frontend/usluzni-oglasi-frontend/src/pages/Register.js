import React from 'react'
import { Link } from 'react-router-dom'

const Register = props => {
  return (
    <div>Register
         <Link to='/login'>Go to login.</Link>
        <Link to='/'>Go to home.</Link>
    </div>
  )
}

export default Register