import { type NextRequest, NextResponse } from "next/server"
import { togglePostLike, getUserLikeStatus } from "@/lib/likes-comments"

export const dynamic = "force-dynamic"

export async function POST(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const { id: postId } = params
    const body = await request.json()
    const { userId } = body

    if (!userId) {
      return NextResponse.json({ error: "User ID is required" }, { status: 400 })
    }

    const result = await togglePostLike(postId, userId)

    return NextResponse.json({
      success: true,
      liked: result.liked,
      action: result.action,
    })
  } catch (error) {
    console.error("Error toggling like:", error)
    return NextResponse.json({ error: "Failed to toggle like" }, { status: 500 })
  }
}

export async function GET(request: NextRequest, { params }: { params: { id: string } }) {
  try {
    const { id: postId } = params
    const { searchParams } = request.nextUrl
    const userId = searchParams.get("userId")

    if (!userId) {
      return NextResponse.json({ liked: false })
    }

    const liked = await getUserLikeStatus(postId, userId)

    return NextResponse.json({ liked })
  } catch (error) {
    console.error("Error checking like status:", error)
    return NextResponse.json({ liked: false })
  }
}
