import React from 'react'
import { Link } from 'react-router-dom'

const Home = props => {
  return (
    <div>Home
        <Link to='/login'>Go to login.</Link>
        <Link to='/registration'>Go to registration.</Link>
    </div>

  )
}

export default Home